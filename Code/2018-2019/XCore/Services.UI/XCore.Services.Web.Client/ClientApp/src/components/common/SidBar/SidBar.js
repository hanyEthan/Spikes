import React, { Component, Fragment } from 'react'
import  "../../../assets/css/SideBar.css";
import { useRouteMatch,Link,Switch,Route } from "react-router-dom";
import AppForm from '../../../views/Form/AppForm';
export   default function  SidBar() 
{
		const { path, url } = useRouteMatch();
        return (
          <Fragment>
			    <div >
				<div className="col-xs col-md-1 sidebar">
					<nav>
						<ul className="nav nav-pills nav-stacked">
							<li className="nav-link"><Link to={`${url}/AppForm`} >AppForm</Link></li>
							<li className="nav-link"><Link to={`${url}/About`} >About</Link></li>
							<li className="nav-link"><Link to={`${url}/Contacts`}>Contacts</Link></li>
						</ul>
					</nav>
				</div>
				<Switch>
				<Route exact path={path}>
				<h3>Please select a Sub Routing From SidBar.</h3>
				</Route>
				<Route path={`${path}/:Formid`}>
				<AppForm />
				</Route>
			</Switch>
			</div>
           </Fragment>
        )
    }

