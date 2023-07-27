//
//  NativeWidget.swift
//  NativeWidget
//
//  Created by Piotr Romanowski on 22/07/2023.
//

import WidgetKit
import SwiftUI

struct AttendanceReport: Codable {
    let percentage: Float
}

struct AttendanceTimelineProvider: TimelineProvider {
    func placeholder(in context: Context) -> AttendanceEntry {
        AttendanceEntry(date: Date(), percentage: 78.88)
    }

    func getSnapshot(in context: Context, completion: @escaping (AttendanceEntry) -> ()) {
        let entry = AttendanceEntry(date: Date(), percentage: 78.88)
        completion(entry)
    }

    func getTimeline(in context: Context, completion: @escaping (Timeline<AttendanceEntry>) -> ()) {
        var entries: [AttendanceEntry] = []
        
        let jsonData = readWidgetData(fileName: "attendance-stats.json", defaultValue: AttendanceReport(percentage: 0));
        
        entries.append(AttendanceEntry(date: Date(), percentage: jsonData.percentage))
        
        let timeline = Timeline(entries: entries, policy: .after (Date().addingTimeInterval(15 * 60)))
        completion(timeline)
    }
}

struct AttendanceEntry: TimelineEntry {
    let date: Date
    let percentage: Float
}

struct AttendanceWidgetEntryView : View {
    var entry: AttendanceTimelineProvider.Entry

    var body: some View {
        VStack(alignment: .trailing) {
                Spacer()
            HStack() {
                Spacer()
                VStack(alignment: .trailing) {
                    Text("Frekwencja")
                    Text(String(format: "%.2f%%", entry.percentage)).font(.headline)
                }.padding(10)
            }
        }.padding(.leading)
    }
}

struct AttendanceWidget: Widget {
    let kind: String = "AttendanceWidget"

    var body: some WidgetConfiguration {
        StaticConfiguration(kind: kind, provider: AttendanceTimelineProvider()) { entry in
            AttendanceWidgetEntryView(entry: entry)
        }
        .supportedFamilies([.systemSmall])
        .configurationDisplayName("Frekwencja")
        .description("Sumaryczna frekwencja.")
    }
}

struct AttendanceWidget_Previews: PreviewProvider {
    static var previews: some View {
        AttendanceWidgetEntryView(entry: AttendanceEntry(date: Date(), percentage: 78.88))
            .previewContext(WidgetPreviewContext(family: .systemSmall))
    }
}
