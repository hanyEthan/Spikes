import ConfigurationWatcher from "./ConfigurationSaga";
import { fork,all } from "redux-saga/effects";

export function* rootSaga () {
    yield all([
        fork(ConfigurationWatcher) // saga1 can also yield [ fork(actionOne), fork(actionTwo) ]
       
    ]);
}