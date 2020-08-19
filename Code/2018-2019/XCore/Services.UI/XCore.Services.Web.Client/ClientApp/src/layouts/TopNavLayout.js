import React, { Component,Fragment } from 'react';
import Navbar from '../components/common/NavBar/Navbar';
import MainLayout from './MainLayout';

export default class TopNavLayout extends Component {
    constructor(props){
        super(props);
    }
    render() {
        return (
            <Fragment>
                <Navbar />
                <MainLayout>
                  {this.props.children}
                </MainLayout>
            </Fragment>
        )
    }
}
