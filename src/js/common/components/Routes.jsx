import React from 'react';
import Router, {Route, RouteHandler, Link, DefaultRoute, NotFoundRoute} from 'react-router';
import App, {About, Inbox, NotFound} from "./App";

var routes = (
    <Route handler={App}>
        <DefaultRoute handler={About}/>
        <Route name="about" path="about" handler={About}/>
        <Route name="inbox" path="inbox" handler={Inbox}/>
        <NotFoundRoute handler={NotFound} />
    </Route>
);

export default routes;