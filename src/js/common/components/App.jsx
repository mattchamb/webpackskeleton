import React from 'react';
import {RouteHandler, Link} from 'react-router';
import message from '../lib/hello';

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

var NotFound = React.createClass({
    render: function () {
        return <p>Page not found.</p>;
    }
});

var App = React.createClass({
    render () {
        return (
            <div>
                <h1>App</h1>
                <h2>{message}</h2>
                <ul>
                    <li><Link to="about">About</Link></li>
                    <li><Link to="inbox">Inbox</Link></li>
                </ul>
                <RouteHandler/>
            </div>
        )
    }
});

export { About, Inbox, NotFound };
export default App;