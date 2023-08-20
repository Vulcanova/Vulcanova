export interface StudentPayload {
  topLevelPartition: string;
  partition: string;
  classDisplay: string;
  infoDisplay: string;
  fullSync: boolean;
  login: Login;
  unit: Unit;
  constituentUnit: ConstituentUnit;
  capabilities: string[];
  educatorList: Educator[];
  pupil: Pupil;
  periods: Period[];
  journal: Journal;
  constraints: Constraints;
  state: number;
  context: string;
  messageBox: MessageBox;
}

export interface ConstituentUnit {
  id: number;
  short: string;
  name: string;
  address: string;
  patron: string;
  schoolTopic: string;
}

export interface Constraints {
  absenceDaysBefore: any;
  absenceHoursBefore: any;
}

export interface Educator {
  globalKey: string;
  name: string;
  group: null;
}

export interface Role {
  roleName: string;
  roleOrder: number;
  address: string;
  addressHash: string;
  unitSymbol: any;
  constituentUnitSymbol: string;
  classSymbol: string;
  name: string;
  surname: string;
  initials: string;
}

export interface Journal {
  id: number;
  yearStart: YearEnd;
  yearEnd: YearEnd;
}

export interface YearEnd {
  timestamp: number;
  dateDisplay: string;
}

export interface Login {
  id: number;
  value: string;
  firstName: string;
  secondName: string;
  surname: string;
  displayName: string;
  loginRole: string;
}

export interface Period {
  capabilities: any[];
  id: number;
  level: number;
  number: number;
  start: YearEnd;
  end: YearEnd;
  current: boolean;
  last: boolean;
}

export interface Pupil {
  id: number;
  loginId: number;
  loginValue: string;
  firstName: string;
  secondName: string;
  surname: string;
  sex: boolean;
}

export interface Unit {
  id: number;
  symbol: string;
  short: string;
  RestURL: string;
  name: string;
  address: string;
  patron: string;
  displayName: string;
  schoolTopic: string;
}

export interface MessageBox {
  id: number;
  globalKey: string;
  name: string;
}

export interface Status {
  code: number;
  message: string;
}
