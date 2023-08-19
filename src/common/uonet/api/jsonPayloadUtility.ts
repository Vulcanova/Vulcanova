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
