import { FETCH_CONFIG_FAILUR,FETCH_CONFIG_REQUEST,FETCH_CONFIG_SUCCESS ,SEARCH_CONFIG_RESULT} from '../../../helpers/Constants/Types';
const LoadingReducer = (state = false, action) => {
    switch (action.type) {
        case FETCH_CONFIG_REQUEST:
            return true;
        case FETCH_CONFIG_SUCCESS:
            return false;
        case FETCH_CONFIG_FAILUR:
            return false;
        default:
            return state;
    }
};
export default LoadingReducer;
