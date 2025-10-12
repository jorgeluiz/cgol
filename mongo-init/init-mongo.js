db = db.getSiblingDB('admin');
db.auth('root', 'root');

//creating default databases
db = db.getSiblingDB('cgol');
db = db.getSiblingDB('cgol_dev');

print("MongoDB was initied");
