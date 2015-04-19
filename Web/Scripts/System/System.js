var System = System || {};

function registerNamespace(ns) {
    var parts = ns.split('.'),
        parent = System,
        pl, i;
    if (parts[0] == "System") {
        parts = parts.slice(1);
    }
    pl = parts.length;
    for (i = 0; i < pl; i++) {
        //create a property if it doesnt exist
        if (typeof parent[parts[i]] == 'undefined') {
            parent[parts[i]] = {};
        }
        parent = parent[parts[i]];
    }
    return parent;
}