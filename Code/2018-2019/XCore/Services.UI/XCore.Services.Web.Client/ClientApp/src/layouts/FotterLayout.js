import React, { Component, Fragment } from 'react'
import SideBarLayout from './SideBarLayout'
import Fotter from '../components/common/Fotter/Fotter';

export default class FotterLayout extends Component {
    render() {
        return (
          <Fragment>
              <SideBarLayout>
                  {this.props.children}
              </SideBarLayout>
              <Fotter/>
          </Fragment>
        )
    }
}
