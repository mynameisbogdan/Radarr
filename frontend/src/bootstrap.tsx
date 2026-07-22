import { createBrowserHistory } from 'history';
import React from 'react';
import { createRoot, Root } from 'react-dom/client';
import createAppStore from 'Store/createAppStore';
import App from './App/App';

import 'Diag/ConsoleApi';

let root: Root | null = null;

export async function bootstrap() {
  const history = createBrowserHistory();
  const store = createAppStore(history);

  const container = document.getElementById('root');
  if (!container) {
    throw new Error('Missing #root element');
  }

  root ??= createRoot(container);
  root.render(<App store={store} history={history} />);
}
