import React from 'react';
import Router, {Route, RouteHandler, Link, DefaultRoute} from 'react-router';


var About = React.createClass({
    render: function () {
        return <h2>About</h2>;
    }
});

var Inbox = React.createClass({
    render: function () {
        return <h2>Inbox</h2>;
    }
});


var App = React.createClass({
    render () {
        return (
            <div>
                <h1>App</h1>
                <ul>
                    <li><Link to="about">About</Link></li>
                    <li><Link to="inbox">Inbox</Link></li>
                </ul>
                <RouteHandler/>
            </div>
        )
    }
});

var routes = (
    <Route handler={App}>
        <DefaultRoute handler={About}/>
        <Route name="about" path="about" handler={About}/>
        <Route name="inbox" path="inbox" handler={Inbox}/>
    </Route>
);

Router.run(routes, Router.HistoryLocation, (Root) => {
    React.render(<Root/>, document.getElementById('react-content'));
});

