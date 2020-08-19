import React, { Component, Fragment } from 'react'
import SidBar from '../components/common/SidBar/SidBar'
import TopNavLayout from './TopNavLayout'

export default class SideBarLayout extends Component {
    constructor(props)
    {
        super(props);
    }

    render() {
        return (
            <Fragment>
              
                <TopNavLayout>
              
                {this.props.children}
          
                </TopNavLayout>
                <SidBar/>
            </Fragment>
        )
    }
}
