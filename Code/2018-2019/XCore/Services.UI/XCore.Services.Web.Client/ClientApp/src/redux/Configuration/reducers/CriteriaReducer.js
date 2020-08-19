import { SEARCH_CONFIG_RESULT} from '../../../helpers/Constants/Types';
import ConfigSearchCriteria from "../../../models/Configuration/ConfigSearchCriteria";
const CriteriaReducer = (state = new ConfigSearchCriteria() , action) => {
    if (action.type === SEARCH_CONFIG_RESULT) {
        return action.searchValue;
    }
    return state;
};

export default CriteriaReducer;
