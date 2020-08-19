import React, { Component } from 'react'
import {  LoadInitialSearchCriteria} from './ConfigurationHandler';
import { connect } from "react-redux";
import ConfigSearchCriteria from '../../models/Configuration/ConfigSearchCriteria';
import {  LOCAL_STORAGE_SEARCH_CRITERIA} from "../../helpers/Constants/Keys";
import 'font-awesome/css/font-awesome.min.css';
import { fetchConfigRequest } from '../../redux/Configuration/actions';
import ConfigurationRequest from '../../models/Configuration/ConfigurationRequest';
class ConfigCriteriaComponent extends Component {
 // props ...

  // cst ...
  constructor(props) {
    debugger;
        super(props);
        this.Initialize();
        this.state={
          SearchCriteriaState:new ConfigSearchCriteria()
        }
      }
      
     HandleOnchage=(event)=>
    {
      debugger;
        const { value } = event.target;
        let newState = Object.assign({}, this.state);
        newState.SearchCriteriaState.name = value;
        this.setState(newState);
      
      
    }
    HandleOnClick=()=>
    {
          //local storage in browser to save user data after closing browser or make refresh
          localStorage.setItem(LOCAL_STORAGE_SEARCH_CRITERIA,JSON.stringify(this.state.SearchCriteriaState));
      
          LoadInitialSearchCriteria(this.state.SearchCriteriaState);//BL
          var criteria=this.state.SearchCriteriaState;
          var PaginationValue=this.props.PaginationValue;
          var Configuration_Request=new ConfigurationRequest(criteria,PaginationValue);
          this.props.FetchConfigs(Configuration_Request);
    }
   

    // helpers ...
    Initialize=()=>
    {
      debugger;
      LoadInitialSearchCriteria(this.props.SearchValue);//BL
    }
    
    render() {
        return (
              <div className="input-group col-md-4">
              <input className="form-control py-2" type="search" placeholder="Search" id="example-search-input" onChange={this.HandleOnchage} />
              <span className="input-group-append">
                <button className="btn btn-outline-secondary" type="button" onClick={this.HandleOnClick}>
                    <i className="fa fa-search"></i>
                </button>
              </span>
             </div>
        
        )
    }
}

const MapStateToProps =(state)=>
{
 return{
    SearchValue:state.Configs.SearchValue,
    PaginationValue:state.Configs.PaginationValue,
 }
}
const MapDispatchToProps=(dispatch)=>
{return{
  FetchConfigs:(Criteria)=> dispatch(fetchConfigRequest(Criteria))

}
}
export default connect(MapStateToProps,MapDispatchToProps)( ConfigCriteriaComponent);