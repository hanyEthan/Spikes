import React, { Component,Fragment } from 'react'

export default class MainLayout extends Component {
    constructor(props)
    {
        super(props);
    }

    render() {
        return (
        <Fragment>
        <div className="fixed-background" />
        <main>
          <div className="container">{this.props.children}</div>
        </main>
      </Fragment>
        )
    }
}
