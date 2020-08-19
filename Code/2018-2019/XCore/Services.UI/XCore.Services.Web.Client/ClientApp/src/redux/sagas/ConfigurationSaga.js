import { take,call,put,select ,takeEvery} from "redux-saga/effects";//effects
import { FETCH_CONFIG_REQUEST,SEARCH_CONFIG_RESULT } from "../../helpers/Constants/Types";//constatants types
import { SetConfigSuccess,SetConfigFailur } from "../Configuration/actions";
import { FetchConfigs } from "../../services/Configuration/Service";
import { SetConfigSuccessBL } from "../../views/Configuration/ConfigurationHandler";
import ConfigSearchCriteria from "../../models/Configuration/ConfigSearchCriteria";
import axios from 'axios';
import { GetConfigsURL } from "../../helpers/Constants/URLS";
const GetSearchCriteria=(state)=>
{
debugger;
  return state.Configs.SearchValue;
}

//1)generator function  watcher saga
function* ConfigurationWatcher() {
  debugger;
    yield takeEvery(FETCH_CONFIG_REQUEST,ConfigurationWorker);

}

//2)  worker saga
function* ConfigurationWorker(action) {
    debugger;
    try {
        const SearchConfigCriteria =action.payload;//yield select( GetSearchCriteria);
       const Response = yield call(FetchConfigs, SearchConfigCriteria);
       //const Configs = yield call([axios,axios.post],GetConfigsURL ,SearchConfigCriteria);
        debugger;
        var ConfigsTransformed= SetConfigSuccessBL(Response.applist);
        yield put(SetConfigSuccess(ConfigsTransformed,Response.totalRowsCount));
    } catch (error) {
        //dispatch error
        yield put(SetConfigFailur(error.toString()));
    }
}

export default ConfigurationWatcher;
