import React, { Fragment } from 'react';
import {Link } from 'react-router-dom';
import  "../../../assets/css/Navbar.css";
import  "../../../assets/css/images.css";

import ConfigCriteriaComponent from '../../../views/Configuration/ConfigCriteriaComponent';
import MOF_LOGO from "../../../assets/images/logo.svg";

export default function Navbar() {
    return (
        <Fragment>
            <nav className="navbar navbar-light NavStyle" >
                <ul className="navbar-nav mr-auto FlexDirection">
                    <li>
                        <Link  to={'/'} className="nav-link">
                          <img className="MOFLogo" src={MOF_LOGO} />
                        </Link>
                    </li>
                    <li>
                        <Link to={'/Config'} className="nav-link">
                            Config
                        </Link>
                    </li>   
                </ul>
                <ConfigCriteriaComponent />
            </nav>
        </Fragment>
    )
}
