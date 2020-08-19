import {createStore,applyMiddleware,compose} from 'redux';
import {createLogger  } from "redux-logger";
import rootReducer from '../Store/rootReducer';
import createSagaMiddleware from 'redux-saga';
import { rootSaga } from '../sagas/rootSaga';


const configureStore = () => {
    debugger;
    const logger = createLogger();
    //1)create a saga middleware
    const sagaMiddleware = createSagaMiddleware();
    const store = createStore(
        rootReducer,
        compose(
            applyMiddleware(sagaMiddleware, logger), //2)apply middleware saga
            window.__REDUX_DEVTOOLS_EXTENSION__ &&
                window.__REDUX_DEVTOOLS_EXTENSION__(),
        ),
    );
    //3)run saga middleware
    //4) pass root saga to run function
    sagaMiddleware.run(rootSaga);

    return store;
};





export default configureStore  ;
