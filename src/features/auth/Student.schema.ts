import {Realm} from '@realm/react';

export class Student extends Realm.Object<Student> {
  id!: Realm.BSON.UUID;
  pupil!: Pupil;
  unit!: Unit;
  constituentUnit!: ConstituentUnit;
  isActive!: boolean;
  messageBox!: MessageBox;
  periods!: Realm.List<Period>;
  identityThumbprint!: string;
  login!: Login;
  capabilities!: string[];
  classDisplay!: string;
  context!: string;
  partition!: string;

  static schema: Realm.ObjectSchema = {
    name: 'Student',
    properties: {
      id: {type: 'uuid', default: () => new Realm.BSON.UUID()},
      pupil: 'Pupil',
      unit: 'Unit',
      constituentUnit: 'ConstituentUnit',
      isActive: 'bool',
      messageBox: 'MessageBox',
      periods: {
        type: 'list',
        objectType: 'Period',
      },
      identityThumbprint: 'string',
      login: 'Login',
      capabilities: {type: 'list', objectType: 'string'},
      classDisplay: 'string',
      context: 'string',
      partition: 'string',
    },
    primaryKey: 'id',
  };
}

export class Pupil extends Realm.Object<Pupil> {
  id!: number;
  loginId!: number;
  loginValue?: string;
  firstName!: string;
  secondName?: string;
  surname!: string;
  sex!: boolean;

  static schema = {
    name: 'Pupil',
    embedded: true,
    properties: {
      id: 'int',
      loginId: 'int',
      loginValue: 'string?',
      firstName: 'string',
      secondName: 'string?',
      surname: 'string',
      sex: 'bool',
    },
  };
}

export class Unit extends Realm.Object<Pupil> {
  id!: number;
  symbol!: string;
  short!: string;
  restUrl!: string;
  name!: string;
  address!: string;
  patron!: string;
  displayName!: string;
  schoolTopic!: string;

  static schema = {
    name: 'Unit',
    embedded: true,
    properties: {
      id: 'int',
      short: 'string',
      name: 'string',
      address: 'string',
      patron: 'string',
      schoolTopic: 'string',
    },
  };
}

export class ConstituentUnit extends Realm.Object<ConstituentUnit> {
  id!: number;
  short!: string;
  name!: string;
  address!: string;
  patron!: string;
  schoolTopic!: string;

  static schema = {
    name: 'ConstituentUnit',
    embedded: true,
    properties: {
      id: 'int',
      short: 'string',
      name: 'string',
      address: 'string',
      patron: 'string',
      schoolTopic: 'string',
    },
  };
}

export class Period extends Realm.Object<Period> {
  id!: number;
  level!: number;
  number!: number;
  current!: boolean;
  start!: Date;
  end!: Date;

  static schema = {
    name: 'Period',
    embedded: true,
    properties: {
      id: 'int',
      level: 'int',
      number: 'int',
      current: 'bool',
      start: 'date',
      end: 'date',
    },
  };
}

export class MessageBox extends Realm.Object<MessageBox> {
  id!: number;
  globalKey!: string;
  name!: string;

  static schema = {
    name: 'MessageBox',
    embedded: true,
    properties: {
      id: 'int',
      globalKey: 'string',
      name: 'string'
    }
  }
}

export class Login extends Realm.Object<Login> {
  id!: number;
  value!: string;
  firstName!: string;
  secondName?: string;
  surname!: string;
  displayName!: string;
  loginRole!: string;

  static schema = {
    name: 'Login',
    embedded: true,
    properties: {
      id: 'int',
      value: 'string',
      firstName: 'string',
      secondName: 'string?',
      surname: 'string',
      displayName: 'string',
      loginRole: 'string',
    },
  };
}
