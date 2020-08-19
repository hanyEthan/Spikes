
import { FETCH_CONFIG_FAILUR,FETCH_CONFIG_REQUEST,FETCH_CONFIG_SUCCESS } from '../../../helpers/Constants/Types';
const errorReducer = (state = false, action) => {
    switch (action.type) {
        case FETCH_CONFIG_FAILUR:
            return action.error;
        case FETCH_CONFIG_SUCCESS:
        case FETCH_CONFIG_REQUEST:
            return null;
        default:
            return state;
    }
};
export default errorReducer;
