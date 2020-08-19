import ConfigSearchCriteria from "../../models/Configuration/ConfigSearchCriteria";
import { LOCAL_STORAGE_SEARCH_CRITERIA } from "../../helpers/Constants/Keys";
import configureStore from '../../redux/Store/store';
import {SetConfigCriteria,fetchConfigRequest, SetConfigFailur, SetConfigSuccess, SetConfigPaginaion} from '../../redux/Configuration/actions'
  const store = configureStore();
  // props ...
  var defaultCriteria = new ConfigSearchCriteria();

  // ...helpers
  const _LoadConfigData=(criteraModel)=>
   {
       debugger;
      // store.dispatch( SetConfigCriteria(criteraModel));
       store.dispatch( fetchConfigRequest());
       
       
   }
   const _LoadInitialSearchCriteria=(criteraModel)=>
   {
       debugger;
    
      
       store.dispatch( SetConfigCriteria(criteraModel));
    
       //store.dispatch( fetchConfigRequest());

       
       
   }
   const _LoadInitialPagination=(PaginationValue)=>
   {
       debugger;
    
      
       store.dispatch( SetConfigPaginaion(PaginationValue));

   }
   export const LoadInitialPagination=(PaginationValue)=>{
       _LoadInitialPagination(PaginationValue);
   }
    export const LoadInitialSearchCriteria=(criteriaInState)=>{
        try{
        
        
            debugger;
            // 1 . check criteria in redux
            if(criteriaInState != null)
            {
                _LoadInitialSearchCriteria(criteriaInState);
                return true;
            }
            // 2 . check Loacal Storage cache, and set it in redux if found (ignore)
            var storedCriteria= JSON.parse(localStorage.getItem(LOCAL_STORAGE_SEARCH_CRITERIA));
            if(storedCriteria!=null)
            {
                _LoadInitialSearchCriteria(storedCriteria);
                return true;
            }
            // 3 . get default criteria model, and store it in redux
            if(defaultCriteria!=null)
            {
                _LoadInitialSearchCriteria(defaultCriteria);

                return true;

            }

        }
        catch(error)
        {
            return false;
        }
    }
    export const CheckCriteriaValue=(criteriaInState)=>{
        debugger;
            // 1 . check criteria in redux
            if(criteriaInState != null)
            {
                return criteriaInState;
            }
            // 2 . check Loacal Storage cache, and set it in redux if found (ignore)
            var storedCriteria= JSON.parse(localStorage.getItem(LOCAL_STORAGE_SEARCH_CRITERIA));
            if(storedCriteria!=null)
            {
                return storedCriteria;
            }
            // 3 . get default criteria model, and store it in redux
            if(defaultCriteria!=null)
            {
            
                return defaultCriteria;

            }
    }
 
        // publics ...
    export const LoadConfigData=(criteraModel)=>
        {
            debugger;
          var SerchValueNotNull=  LoadInitialSearchCriteria(criteraModel);
          if(SerchValueNotNull)
                _LoadConfigData(criteraModel);
           
        
        }


  

    export const SetConfigSuccessBL=(configs) =>
    {
       var configs=TransformConfigData(configs);
       return configs;
    }

    const TransformConfigData=(configs)=>
    {
        debugger;
        let arr  = [];

        for(var i=0;i<configs.length;i++)
        {
            arr[i]=flattenObject(configs[i]);
        }

    return arr;
    }


    const flattenObject = (ob)=>
    {

        var toReturn = {};
        
        for (var i in ob) {
            if (!ob.hasOwnProperty(i)) continue;
            
            if (typeof( ob[i]) == 'object') {
                var flatObject = flattenObject(ob[i]);
                for (var x in flatObject) {
                    if (!flatObject.hasOwnProperty(x)) continue;
                
                    var label=i + '.' + x;
                    label = label.replace(/[0-9]/g, '');
                    label=  label.replace('.','');
                    toReturn[label] = flatObject[x];
                }
            } else {
                toReturn[i] = ob[i];
            }
        }
        return toReturn;
    };