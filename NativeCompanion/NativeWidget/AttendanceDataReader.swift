//
//  AttendanceDataReader.swift
//  NativeWidgetExtensionExtension
//
//  Created by Piotr Romanowski on 22/07/2023.
//

import Foundation

struct AttendanceReport: Codable {
    let percentage: Float
}

func readAttendanceData() -> AttendanceReport {
    if let url = FileManager.default.containerURL(forSecurityApplicationGroupIdentifier: "group.io.github.vulcanova") {
        let path = url.appendingPathComponent("attendance-stats.json");
        let data = try? String(contentsOf: path);
        if let data = data {
            let jsonData = data.data(using: .utf8)!
            let value = try? JSONDecoder().decode(AttendanceReport.self, from: jsonData);
            
            if let value = value {
                return value;
            }
        }
    }
    return AttendanceReport (percentage: 0)
}
