import loadingReducer from './loadingReducer';
import errorReducer from './errorReducer';
import ConfigReducer from './ConfigReducer';
import pageReducer from './pageReducer';
import CriteriaReducer from './CriteriaReducer';


import { combineReducers } from 'redux';

const rootReducer = combineReducers({
    isLoading: loadingReducer,
    configs: ConfigReducer,
    error: errorReducer,
    Criteria: CriteriaReducer,
    pageNum: pageReducer,
});
export default rootReducer;
