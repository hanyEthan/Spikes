import React,{Fragment} from 'react';
import { Router,Route,Switch , useParams,useRouteMatch } from "react-router-dom";
import IndexConfig from './views/Configuration';
import history  from  './helpers/Utilties/history';
import Home from './views/Home';
import Breadcrumb from 'react-bootstrap/Breadcrumb';
import SidBar from './components/common/SidBar/SidBar';
import AppForm from './views/Form/AppForm';

function App() {
  console.log("history:= ",history.location);
  const  Routes=[
    {path:'/',exact:true,sidbar:()=><div></div>,main:()=><div><Home/></div>},
    {path:'/Config',sidbar:()=><div><SidBar/></div>,main:()=><div><IndexConfig/></div>},
  
  
]
  return (
     <Fragment>
        <Router history ={history} >
          <Switch>
               { Routes.map(route => <Route key={route.path} exact={route.exact} path={route.path}  component={route.main}  />)}
               
            </Switch>
        </Router>
                    
     </Fragment>
  );
}

export default App;
