import {Realm} from '@realm/react';

export class Student extends Realm.Object<Student> {
  id!: Realm.BSON.UUID;
  pupil!: Pupil;
  unit!: Unit;
  constituentUnit!: ConstituentUnit;
  isActive!: boolean;
  periods!: Realm.List<Period>;
  identityThumbprint!: string;
  login!: Login;
  capabilities!: string[];
  senderEntry!: SenderEntry;
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
      periods: {
        type: 'list',
        objectType: 'Period',
      },
      identityThumbprint: 'string',
      login: 'Login',
      capabilities: {type: 'list', objectType: 'string'},
      senderEntry: 'SenderEntry',
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
  restURL!: string;
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
      symbol: 'string',
      restURL: 'string',
      short: 'string',
      name: 'string',
      address: 'string',
      patron: 'string',
      displayName: 'string',
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

export class SenderEntry extends Realm.Object<SenderEntry> {
  loginId!: number;
  address!: string;
  addressHash!: string;
  initials!: string;

  static schema = {
    name: 'SenderEntry',
    embedded: true,
    properties: {
      loginId: 'int',
      address: 'string',
      addressHash: 'string',
      initials: 'string',
    },
  };
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
