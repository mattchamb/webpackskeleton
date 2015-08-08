import React from 'react';
import Router, {Route, RouteHandler, Link, DefaultRoute} from 'react-router';
import routes from '../common/components/Routes';

function PreRender(uri, cb) {
    Router.run(routes, uri, (Root) => {
        try {
            var result = React.renderToString(<Root/>);
            console.log(result);
            cb(null, result);
        } catch(ex) {
            cb(ex);
        }
    });
}


export default PreRender;