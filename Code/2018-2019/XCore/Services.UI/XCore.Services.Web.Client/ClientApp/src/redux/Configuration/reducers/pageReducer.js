import { FETCH_CONFIG_FAILUR,FETCH_CONFIG_REQUEST,FETCH_CONFIG_SUCCESS ,SEARCH_CONFIG_RESULT} from '../../../helpers/Constants/Types';
const pageReducer = (state = 1, action) => {
    switch (action.type) {
        case FETCH_CONFIG_SUCCESS:
            return state + 1;

        default:
            return state;
    }
};
export default pageReducer;
