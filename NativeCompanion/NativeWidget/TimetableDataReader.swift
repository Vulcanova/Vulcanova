//
//  TimetableDataReader.swift
//  NativeWidgetExtension
//
//  Created by Piotr Romanowski on 24/07/2023.
//

import Foundation

struct TimetableElement: Codable {
    let key: Date
    let value: [TimetableLesson]
}

struct TimetableLesson: Codable {
    let no: Int
    let subjectName, teacherName: String
    let date, start, end: Date
    let roomName: String?
}

struct OverridableValue<T : Codable>: Codable {
    let originalValue, value: T
}

typealias TimetableData = [TimetableElement]

func readTimetableData() -> TimetableData {
    if let url = FileManager.default.containerURL(forSecurityApplicationGroupIdentifier: "group.io.github.vulcanova") {
        let path = url.appendingPathComponent("timetable.json");
        let data = try? String(contentsOf: path);
        if let data = data {
            let jsonData = data.data(using: .utf8)!
            do {
                let decoder = JSONDecoder()
                decoder.dateDecodingStrategy = .iso8601
                let value = try decoder.decode(TimetableData.self, from: jsonData);
                return value;
            } catch {
                print(error)
                return [];
            }
        }
    }
    return []
}
