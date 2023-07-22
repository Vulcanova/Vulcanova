//
//  NativeWidget.swift
//  NativeWidget
//
//  Created by Piotr Romanowski on 22/07/2023.
//

import WidgetKit
import SwiftUI

struct Provider: TimelineProvider {
    func placeholder(in context: Context) -> AttendanceEntry {
        AttendanceEntry(date: Date(), percentage: 78.88)
    }

    func getSnapshot(in context: Context, completion: @escaping (AttendanceEntry) -> ()) {
        let entry = AttendanceEntry(date: Date(), percentage: 78.88)
        completion(entry)
    }

    func getTimeline(in context: Context, completion: @escaping (Timeline<AttendanceEntry>) -> ()) {
        var entries: [AttendanceEntry] = []
        
        let jsonData = readAttendanceData();
        
        entries.append(AttendanceEntry(date: Date(), percentage: jsonData.percentage))
        
        let timeline = Timeline(entries: entries, policy: .after (Date().addingTimeInterval(15 * 60)))
        completion(timeline)
    }
}

struct AttendanceEntry: TimelineEntry {
    let date: Date
    let percentage: Float
}

struct NativeWidgetEntryView : View {
    var entry: Provider.Entry

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

struct NativeWidget: Widget {
    let kind: String = "NativeWidget"

    var body: some WidgetConfiguration {
        StaticConfiguration(kind: kind, provider: Provider()) { entry in
            NativeWidgetEntryView(entry: entry)
        }
        .configurationDisplayName("Frekwencja")
        .description("Sumaryczna frekwencja.")
    }
}

struct NativeWidget_Previews: PreviewProvider {
    static var previews: some View {
        NativeWidgetEntryView(entry: AttendanceEntry(date: Date(), percentage: 78.88))
            .previewContext(WidgetPreviewContext(family: .systemSmall))
    }
}
