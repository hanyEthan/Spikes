import ConfigSearchCriteria from "./ConfigSearchCriteria";
import Pagination from "../Pagination/Pagination";

export default class ConfigurationRequest
{
   constructor(_configSearch,_pagination){
    this.configSearch=_configSearch !=null?_configSearch:new ConfigSearchCriteria();
    this.pagination=_pagination!=null?_pagination:new Pagination();
   }
};
