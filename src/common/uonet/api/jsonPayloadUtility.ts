export const stringifyPascalCase = <T>(obj: T): string => {
  return JSON.stringify(obj, (key, value) => {
    if (value && typeof value === 'object') {
      let replacement: Record<string, any> = {};
      for (const k in value) {
        if (Object.hasOwnProperty.call(value, k)) {
          replacement[k.charAt(0).toUpperCase() + k.substring(1)] = value[k];
        }
      }
      return replacement;
    }
    return value;
  });
};

export const parsePascalCase = <T>(json: string): T => {
  return JSON.parse(json, (key, value) => {
    if (value && typeof value === 'object') {
      for (const k in value) {
        if (/^[A-Z]/.test(k) && Object.hasOwnProperty.call(value, k)) {
          value[k.charAt(0).toLowerCase() + k.substring(1)] = value[k];
          delete value[k];
        }
      }
    }
    return value;
  });
};
