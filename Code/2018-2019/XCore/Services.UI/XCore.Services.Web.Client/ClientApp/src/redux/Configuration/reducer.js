
import { FETCH_CONFIG_FAILUR,FETCH_CONFIG_REQUEST,FETCH_CONFIG_SUCCESS ,SEARCH_CONFIG_RESULT, PAGINATION_VALUE} from '../../helpers/Constants/Types';
import Pagination from '../../models/Pagination/Pagination';

const initialState={
    loading:false,
    data:[],
    error:'',  
    SearchValue:null,//new ConfigSearchCriteria()
    PaginationValue:new Pagination(),
    totalRows:0
}

export const ConfigReducer =(state=initialState,action)=>
{
    debugger;
  switch (action.type) {       
      case FETCH_CONFIG_REQUEST:
          return{
              ...state,
              loading:true,
              SearchValue:action.payload.configSearch,
              PaginationValue:action.payload.pagination,
              data:[],
              error:''
          }
        case FETCH_CONFIG_SUCCESS:
            return{
                ...state,
                loading:false,
                data:action.payload.data,
                totalRows:action.payload.totalRows,

                error:''
            }
        case FETCH_CONFIG_FAILUR:
            return{
                ...state,
                loading:false,
                data:[],
                error:action.payload
            }
        case PAGINATION_VALUE:
            return{
                ...state,
                PaginationValue:action.payload
            }
        case SEARCH_CONFIG_RESULT:
                return{
                    ...state,
                    SearchValue:action.payload
                   
                }
           /* return Object.assign({}, state, {
                SearchValue:action.payload,
                data: action.payload? state.data.filter(item => item.id== action.payload):state.data
              })*/
        
        default:
           return state;
  }
}