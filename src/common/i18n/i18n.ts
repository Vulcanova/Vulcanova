import {initReactI18next} from 'react-i18next';
import i18n from 'i18next';
import stringsEn from 'locales/en/strings.json';
import {NativeModules, Platform} from 'react-native';

export const i18nInit = () => {
  const locale =
    Platform.OS === 'ios'
      ? NativeModules.SettingsManager.settings.AppleLocale
      : NativeModules.I18nManager.localeIdentifier;

  i18n.use(initReactI18next).init({
    resources: {
      en: stringsEn,
    },
    ns: Object.keys(stringsEn),
    lng: locale,
    fallbackLng: 'en',
  });
};
