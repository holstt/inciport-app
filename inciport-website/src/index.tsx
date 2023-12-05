import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import reportWebVitals from './reportWebVitals';
import ContextProvider from './ContextProvider';

ReactDOM.render(
	<BrowserRouter>
		<ContextProvider>
			<App />
		</ContextProvider>
	</BrowserRouter>,
	document.getElementById('root')
);

reportWebVitals();
