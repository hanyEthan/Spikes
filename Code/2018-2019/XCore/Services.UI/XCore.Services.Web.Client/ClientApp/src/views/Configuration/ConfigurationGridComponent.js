import React, { Component, Fragment } from 'react'
import Table from '@material-ui/core/Table';
import TableHead from '@material-ui/core/TableHead';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableFooter from '@material-ui/core/TableFooter';
import TablePagination from '@material-ui/core/TablePagination';


import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';

import { connect } from 'react-redux';
import { LoadConfigData, LoadInitialSearchCriteria, CheckCriteriaValue } from './ConfigurationHandler';
import { fetchConfigRequest } from '../../redux/Configuration/actions';
import ProgressBar from '../../components/common/ProgressBar/ProgressBar';
import ConfigurationRequest from '../../models/Configuration/ConfigurationRequest';
import Pagination from '../../models/Pagination/Pagination';
class ConfigurationGridComponent extends Component {
  constructor(props){
    debugger;
    super(props);
this.state={
  PageNum:0,
  PageSize:10

}
  }

  FetchConfigsWithPagination=(PageNum,PageSize)=>{
    var Criteria= CheckCriteriaValue(this.props.SearchValue);
    var PaginationValue=new Pagination();
    PaginationValue.pageNo=PageNum;
    PaginationValue.pageSize=PageSize;
 
    var Configuration_Request=new ConfigurationRequest(Criteria,PaginationValue);
     this.props.FetchConfigs(Configuration_Request);
  }



   handleChangePage = (event, newPage) => {
   // setPage(newPage)
   debugger;
 
   this.setState({PageNum:newPage});
   this.FetchConfigsWithPagination(newPage,this.state.PageSize);
   
  };
   handleChangeRowsPerPage = event => {
   console.log("PageSize event",event.target.value);
   var _PageSize=parseInt(event.target.value, 10);
   this.setState({PageSize:_PageSize});
   this.FetchConfigsWithPagination(this.state.PageNum,_PageSize);
  };
   componentWillMount()
   {
     debugger;
    var Criteria= CheckCriteriaValue(this.props.SearchValue);
    var PaginationValue=this.props.PaginationValue;

    var Configuration_Request=new ConfigurationRequest(Criteria,PaginationValue);
     this.props.FetchConfigs(Configuration_Request);
   }
 
   MapColumNames =(ArrColumns)=>
   {
       var MapedArr=[];
     for(var i=0;i<ArrColumns.length;i++)
     {
       MapedArr[i]={key:i,value:ArrColumns[i]}
     }
    return MapedArr;

   }
   
    render() {
      const Pagination_Value=this.props.PaginationValue;
      const totalRows=this.props.totalRows;

  if(this.props.loading)
  {
    debugger;
    return(<div><ProgressBar/></div>)
  }
  if(this.props.ConfigsArr.length>0)
  {
    debugger;
     console.log("ConfigData",this.props.ConfigsArr);
       var COlHeaders= Object.keys(this.props.ConfigsArr[0]);
       var MappedColHeader=this.MapColumNames(COlHeaders);
       console.log(COlHeaders);
       const emptyRows = this.state.PageSize - Math.min(this.state.PageSize, this.props.totalRows - this.state.PageNum * this.state.PageSize);
    return(
      <Paper >
      <div >
          <Table >
              <TableHead>
                  <TableRow>
                    {MappedColHeader.map(itemHeader=> <TableCell key={itemHeader.key}>{itemHeader.value}</TableCell>)}
                      
                  </TableRow>
              </TableHead>
              <TableBody>
               {this.props.ConfigsArr.map(n => {
                            return (
                                <TableRow key={n.id}>
                                    <TableCell >{n.id}</TableCell>
                                    <TableCell >{n.name}</TableCell>
                                    <TableCell >{n.code}</TableCell>
                                    <TableCell>{n.moduleid}</TableCell>
                                    <TableCell>{n.modulename}</TableCell>
                                    <TableCell >{n.modulecode}</TableCell>
                                    <TableCell>{n.moduleconfigid}</TableCell>
                                    <TableCell>{n.moduleconfigname}</TableCell>
                                    <TableCell >{n.moduleconfigcode}</TableCell>
                                </TableRow>
                            );
                        })}
                        
              </TableBody>
              <TableFooter>
                  
              </TableFooter>
          </Table>
      </div>
      <TablePagination
          rowsPerPageOptions={[5, 10, 25]}
          component="div"
          count={totalRows}
          rowsPerPage={Pagination_Value.pageSize}
          page={Pagination_Value.pageNo}
          backIconButtonProps={{
            'aria-label': 'next page',
            
          }}
          nextIconButtonProps={{
            'aria-label': 'previous page',
          }}
          onChangePage={this.handleChangePage}
          onChangeRowsPerPage={this.handleChangeRowsPerPage}
        />
  </Paper>
        )
  }
return(
  <div>
     No data....
  </div>
)

          
          
    }
}
const MapStateToProps =(state)=>
{
  debugger;
return{
   ConfigsArr:state.Configs.data,
   loading:state.Configs.loading,
   error:state.Configs.error,
   SearchValue:state.Configs.SearchValue,
   PaginationValue:state.Configs.PaginationValue,
   totalRows:state.Configs.totalRows,

   
}

}
const MapDispatchToProps=(dispatch)=>
{return{
  FetchConfigs:(ConfigurationRequest)=> dispatch(fetchConfigRequest(ConfigurationRequest))

}
}
export default connect(MapStateToProps,MapDispatchToProps) (ConfigurationGridComponent);