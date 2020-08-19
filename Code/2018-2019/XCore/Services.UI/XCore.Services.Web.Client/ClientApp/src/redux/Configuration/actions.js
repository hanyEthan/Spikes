
import { FETCH_CONFIG_FAILUR, FETCH_CONFIG_REQUEST, FETCH_CONFIG_SUCCESS,SEARCH_CONFIG_RESULT, PAGINATION_VALUE } from "../../helpers/Constants/Types";
import ConfigSearchCriteria from '../../models/Configuration/ConfigSearchCriteria';


export const SetConfigPaginaion =(PaginationValue)=>
{
    debugger;
    return{
        type:PAGINATION_VALUE,
        payload:PaginationValue
    }
}

export const SetConfigCriteria =(SearchValue)=>
{
    debugger;
    return{
        type:SEARCH_CONFIG_RESULT,
        payload:SearchValue
    }
}

export const fetchConfigRequest =(ConfigurationRequest)=>
{
    debugger;
    return{
        type:FETCH_CONFIG_REQUEST,
        payload:ConfigurationRequest
    }
}

export const SetConfigSuccess =(configs,totalRowsCount)=>
{
    debugger;
    return{
        type:FETCH_CONFIG_SUCCESS,
        payload:{data:configs,totalRows:totalRowsCount}
        
    }
}

export const SetConfigFailur =(error)=>
{
    debugger;
    return{
        type:FETCH_CONFIG_FAILUR,
        payload:error
    }
}