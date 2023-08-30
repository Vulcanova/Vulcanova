import React, {createContext, useContext, useState} from 'react';
import {useStudent} from '../auth/StudentContext';

export interface PeriodContextType {
  activePeriodId: number;
  setActivePeriodId(periodId: number): void;
}

export const PeriodContext = createContext<PeriodContextType | null>(null);

interface Props {
  children: React.ReactNode;
}

export const PeriodProvider = ({children}: Props) => {
  const {activeStudent} = useStudent();
  const [activePeriodId, setActivePeriodId] = useState(
    activeStudent.periods.find(p => p.current)?.id ??
      activeStudent.periods[0].id,
  );

  const value = {activePeriodId, setActivePeriodId};

  return (
    <PeriodContext.Provider value={value}>{children}</PeriodContext.Provider>
  );
};

export const usePeriod = () => useContext(PeriodContext) as PeriodContextType;
