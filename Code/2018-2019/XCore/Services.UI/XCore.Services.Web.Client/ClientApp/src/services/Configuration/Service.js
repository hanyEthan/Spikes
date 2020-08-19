import axios from 'axios';
import {BaseURL, GetConfigsURL } from "../../helpers/Constants/URLS";

export  const FetchConfigs =  async SearchCriteria => {
    debugger;
        
        const response = await  axios.post(BaseURL+GetConfigsURL,SearchCriteria);
        
        const configs=await response.data;
        
        if (response.status >= 400) {
            throw new Error(response.error);
        }
        return configs;
    
}



