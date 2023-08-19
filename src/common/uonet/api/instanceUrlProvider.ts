import axios from 'axios';

export const ROUTING_RULES_URL =
  'https://komponenty.vulcan.net.pl/UonetPlusMobile/RoutingRules.txt';

export type InstanceUrlProvider = (
  token: string,
  symbol: string,
) => Promise<string | null>;

export const getInstanceUrl: InstanceUrlProvider = async (
  token: string,
  symbol: string,
) => {
  const response = await axios.get<string>(ROUTING_RULES_URL);
  const contents = response.data;

  const baseUrl = contents
    .split('\n')
    .find(l => l.startsWith(token.slice(0, 3)))
    ?.split(',')[1]
    .trimEnd();

  if (!baseUrl) {
    return null;
  }

  return baseUrl + '/' + symbol;
};

export const extractInstanceUrlFromRequestUrl = (
  apiEndpointUrl: string,
): string => {
  const url = new URL(apiEndpointUrl);
  return url.origin + '/' + url.pathname.split('/')[1];
};
