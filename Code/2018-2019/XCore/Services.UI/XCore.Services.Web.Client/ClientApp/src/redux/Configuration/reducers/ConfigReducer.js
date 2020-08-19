import { FETCH_CONFIG_SUCCESS } from '../../../helpers/Constants/Types';

const ConfigReducer = (state = [], action) => {
    if (action.type === FETCH_CONFIG_SUCCESS) {
        return [...state, ...action.Configs];
    }
    return state;
};

export default ConfigReducer;
