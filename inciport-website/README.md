# Architecture
The project is structured in the following way:
- components
- services
- models
- CSS
- assets
- UnitTests
- IntegrationTests

In `root` there is the index.tsx file, which is responsible for running the website. It makes use of the ContextProvider.tsx file, which is also in the root folder that allows the use of a context and makes it possible to avoid prop drilling through all the components.
App.tsx defines all the routes for the different pages on the website and what components should be shown when you are redirected to these pages.

In `services` folder are all the that the website uses. This includes the authService that handles authorizing the user and the ApiClient which handles all api requests.

In `models` all the models are placed which are used throughout the website. They are used to map the data that is received through the ApiClient.

`css` contains all the css for the website.

`components` contains all the components that are used in the website. It is spread into three parts. These three are:
- Base components
- Composite components
- Pages

`basecomponents` are the lowest level of components and are responsible for handling one single thing. An example is NameComponent which can be used to display an name. The idea is that these components should be used in the next layer of components which is compositecomponents.

`compositecomponents` are the middle level of components and consists of base components and/or composite components. They are most responsible of adding together other components, such as adding NameComponents and ButtonsComponent and providing the functionality that should happen in these two components.

`pages` are the highest level of components and are the pages that the user will actually and see and are routed to. Pages cannot contain other other pages. They can only contain base and/or composite components.

# Set up

This apps has been created with create-react-app

- https://create-react-app.dev/docs/adding-typescript/

`npx create-react-app my-app --template typescript`

# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can’t go back!**

If you aren’t satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you’re on your own.

You don’t have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn’t feel obligated to use this feature. However we understand that this tool wouldn’t be useful if you couldn’t customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).
