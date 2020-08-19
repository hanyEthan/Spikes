//Main Page
import React, { Component, Fragment } from 'react';
import FotterLayout from "../../layouts/FotterLayout";
import ConfigCriteriaComponent from './ConfigCriteriaComponent';
import ConfigurationGridComponent from './ConfigurationGridComponent';

 class IndexConfig extends Component {
    render() {
debugger;
        return (
           
                <FotterLayout > 
                   <ConfigurationGridComponent />
                </FotterLayout>
            
        )
       
    }
}


export default IndexConfig
