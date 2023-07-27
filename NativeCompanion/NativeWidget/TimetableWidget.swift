//
//  TimetableWidget.swift
//  NativeWidgetExtension
//
//  Created by Piotr Romanowski on 23/07/2023.
//

import WidgetKit
import SwiftUI

struct Provider: TimelineProvider {
    func placeholder(in context: Context) -> TimetableEntry {
        TimetableEntry(date: Date(), previousLesson: nil, currentLesson: nil, futureLessons: [])
    }
    
    func getSnapshot(in context: Context, completion: @escaping (TimetableEntry) -> ()) {
        let entry = TimetableEntry(date: Date(), previousLesson: nil, currentLesson: nil, futureLessons: [])
        completion(entry)
    }
    
    func getTimeline(in context: Context, completion: @escaping (Timeline<TimetableEntry>) -> ()) {
        var entries: [TimetableEntry] = []
        
        let jsonData = readTimetableData();
        
        jsonData.forEach { dayGroup in
            let date = dayGroup.key;
            
            let sortedLessons = dayGroup.value.sorted {
                $0.start < $1.start
            }
            let lessonsInDay = dayGroup.value.count
            
            if (lessonsInDay == 0) {
                entries.append(TimetableEntry(date: date, previousLesson: nil, currentLesson: nil, futureLessons: [], timetableState: .noLessonsThatDay))
            }
            
            for i in 0...(lessonsInDay - 1) {
                let previousLesson = sortedLessons[safelyIndex: i - 1]
                let currentLesson = sortedLessons[i]
                let futureLessons = lessonsInDay - i > 2 ? sortedLessons[(i + 1)...] : []
                
                entries.append(TimetableEntry(date: currentLesson.start, previousLesson:
                                                previousLesson == nil ? nil : TimetableEntry.TimetableEntryLesson.fromTimetableLesson(lesson: previousLesson!),
                                              currentLesson: TimetableEntry.TimetableEntryLesson.fromTimetableLesson(lesson: currentLesson),
                                              futureLessons: futureLessons.map(TimetableEntry.TimetableEntryLesson.fromTimetableLesson)))
            }

            if let lastLessonInDay = sortedLessons.last {
                entries.append(TimetableEntry(date: lastLessonInDay.end, previousLesson: nil, currentLesson: nil, futureLessons: [], timetableState: .lessonsOver))
            }
            
            let startOfDay = Calendar.current.startOfDay(for: Date())
            
            entries.append(TimetableEntry(date: startOfDay, previousLesson: nil, currentLesson: nil, futureLessons: sortedLessons.map(TimetableEntry.TimetableEntryLesson.fromTimetableLesson)))
        }
        
        let timeline = Timeline(entries: entries, policy: .after (Date().addingTimeInterval(15 * 60)))
        completion(timeline)
    }
}


struct TimetableEntry: TimelineEntry {
    let date: Date
    
    let previousLesson: TimetableEntryLesson?
    let currentLesson: TimetableEntryLesson?
    let futureLessons: [TimetableEntryLesson]
    var timetableState: TimetableState = .normal
    
    struct TimetableEntryLesson: Hashable {
        let no: Int
        let name: String
        let classRoom: String?
        let start: String
        let end: String
        
        static let timeFormatter: DateFormatter = {
            let formatter = DateFormatter()
            formatter.timeStyle = .short
            return formatter
        }()
        
        static func fromTimetableLesson(lesson: TimetableLesson) -> TimetableEntryLesson {
            return TimetableEntryLesson(no: lesson.no, name: lesson.subjectName, classRoom: lesson.roomName, start: timeFormatter.string(from: lesson.start), end: timeFormatter.string(from: lesson.end))
        }
    }
    
    enum TimetableState {
        case normal
        case noLessonsThatDay
        case lessonsOver
    }
}

struct TimetableEntryLessonView : View {
    let lesson: TimetableEntry.TimetableEntryLesson
    let style: TimetableEntryStyle
    var showTime: Bool = false
    
    var body: some View {
        HStack() {
            Text("\(lesson.no). \(lesson.name)" + (lesson.classRoom != nil ? " (\(lesson.classRoom!))" : "")).font(.subheadline).foregroundColor(getColor(style: self.style)).lineLimit(1)
            
            Spacer()
            
            if showTime {
                Text("\(lesson.start) - \(lesson.end)").font(.subheadline).foregroundColor(getColor(style: self.style)).lineLimit(1)
            }
        }
    }
    
    enum TimetableEntryStyle {
        case past
        case current
        case future
    }
    
    private func getColor(style: TimetableEntryStyle) -> Color {
        switch style {
        case .past: return Color.gray
        case .current: return Color.blue
        case .future: return Color.primary
        }
    }
}

struct NativeWidgetEntryView : View {
    @Environment(\.widgetFamily) var family
    
    var entry: Provider.Entry
    
    var body: some View {
        
        ZStack(){
            VStack(alignment: .leading) {
                HStack() {
                    VStack(alignment: .leading) {
                        Text("Lekcje dzisiaj").foregroundColor(.blue).bold().padding(.bottom, 2)
                        
                        if entry.timetableState == .lessonsOver {
                            Text("Koniec lekcji 🥳").font(.subheadline)
                        } else if entry.timetableState == .noLessonsThatDay {
                            Text("Brak lekcji dzisiaj 🎊").font(.subheadline)
                        } else {
                            let showTime = family == .systemMedium
                            
                            if let previous = entry.previousLesson {
                                TimetableEntryLessonView(lesson: previous, style: TimetableEntryLessonView.TimetableEntryStyle.past, showTime: showTime)
                            }
                            
                            if let current = entry.currentLesson {
                                TimetableEntryLessonView(lesson: current, style: TimetableEntryLessonView.TimetableEntryStyle.current, showTime: showTime)
                            }
                            
                            let renderFutureLessonsCnt = getRenderFutureLessonsCount()
                            
                            ForEach(0..<renderFutureLessonsCnt) { i in
                                if let futureLesson = entry.futureLessons[safelyIndex: i] {
                                    TimetableEntryLessonView(lesson: futureLesson, style: TimetableEntryLessonView.TimetableEntryStyle.future, showTime: showTime)
                                }
                            }
                            
                            let remainingToRender = entry.futureLessons.count - renderFutureLessonsCnt;
                            
                            if (remainingToRender > 0) {
                                if (remainingToRender > 1) {
                                    Text("...").font(.subheadline).foregroundColor(.gray)
                                }
                                
                                let lastLesson = entry.futureLessons.last!
                                TimetableEntryLessonView(lesson: lastLesson, style: TimetableEntryLessonView.TimetableEntryStyle.future, showTime: showTime)
                            }
                        }
                    }
                    Spacer()
                }
                Spacer()
            }
        }.padding(14)
        
    }
    
    func getRenderFutureLessonsCount() -> Int {
        var futureLessonsRenderCnt = 1;
        
        if entry.previousLesson == nil {
            futureLessonsRenderCnt += 1
        }
        
        if entry.currentLesson == nil {
            futureLessonsRenderCnt += 1
        }
        
        return futureLessonsRenderCnt
    }
}

struct NativeWidget: Widget {
    let kind: String = "NativeWidget"
    
    var body: some WidgetConfiguration {
        StaticConfiguration(kind: kind, provider: Provider()) { entry in
            NativeWidgetEntryView(entry: entry)
        }
        .supportedFamilies([.systemSmall, .systemMedium])
        .configurationDisplayName("Plan lekcji")
        .description("Co dzisiaj?!")
    }
}

struct NativeWidget_Previews: PreviewProvider {
    static var previews: some View {
        NativeWidgetEntryView(entry: TimetableEntry(date: Date(),
                                                    previousLesson: TimetableEntry.TimetableEntryLesson(no: 1, name: "Przyroda", classRoom: "21", start: "8:00", end: "8:45"),
                                                    currentLesson: TimetableEntry.TimetableEntryLesson(no: 2, name: "Edb", classRoom: "37", start: "8:00", end: "8:45"),
                                                    futureLessons: [
                                                        TimetableEntry.TimetableEntryLesson(no: 3, name: "Religia", classRoom: "37", start: "8:00", end: "8:45"),
                                                        TimetableEntry.TimetableEntryLesson(no: 4, name: "Przyroda", classRoom: "37", start: "8:00", end: "8:45"),
                                                        TimetableEntry.TimetableEntryLesson(no: 5, name: "Wizyta papieża", classRoom: "37", start: "8:00", end: "8:45"),
                                                    ],
                                                    timetableState: .normal
                                                   ))
        .previewContext(WidgetPreviewContext(family: .systemMedium))
    }
}

extension Collection {
    subscript(safelyIndex i: Index) -> Element? {
        get {
            guard self.indices.contains(i) else { return nil }
            return self[i]
        }
    }
}
