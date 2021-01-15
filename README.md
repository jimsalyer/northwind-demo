# Northwind Demo

Playground for web technologies using a PostgreSQL version of the Northwind database

## Requirements

- [Node (for NPM scripts in `package.json`)](https://nodejs.org/en/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.Net SDK (version 5.0.2 or above)](https://dotnet.microsoft.com/download/dotnet-core)

## Database

### Start

```shell-script
npm run db-start
```

OR

```shell-script
cd ./db
docker-compose up -d
```

### Stop

```shell-script
npm run db-stop
```

OR

```shell-script
cd ./db
docker-compose down -v
```

## API

### Start

```shell-script
npm run api-start
```

### Start in Watch Mode

```shell-script
npm run api-watch
```

### Run Tests

```shell-script
npm run api-test
```

## Access the API

Enter `http://localhost:5000/swagger` in your browser.

### Query Parameters

- `sorts` is a comma-delimited ordered list of property names to sort by. Adding a - before the name switches to sorting descendingly.
- `filters` is a comma-delimited list of `{Name}{Operator}{Value}` where
  - `{Name}` is the name of a property with the Sieve attribute or the name of a custom filter method for TEntity
    You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if `LikeCount` or `CommentCount` is `>10`
  - `{Operator}` is one of the [Operators](#operators)
  - `{Value}` is the value to use for filtering
    You can also have multiple values (for OR logic) by using a pipe delimiter, eg. `Title@=new|hot` will return posts with titles that contain the text "new" or "hot"

### Operators

| Operator | Meaning                                      |
| -------- | -------------------------------------------- |
| `==`     | Equals                                       |
| `!=`     | Not equals                                   |
| `>`      | Greater than                                 |
| `<`      | Less than                                    |
| `>=`     | Greater than or equal to                     |
| `<=`     | Less than or equal to                        |
| `@=`     | Contains                                     |
| `_=`     | Starts with                                  |
| `!@=`    | Does not Contains                            |
| `!_=`    | Does not Starts with                         |
| `@=*`    | Case-insensitive string Contains             |
| `_=*`    | Case-insensitive string Starts with          |
| `==*`    | Case-insensitive string Equals               |
| `!=*`    | Case-insensitive string Not equals           |
| `!@=*`   | Case-insensitive string does not Contains    |
| `!_=*`   | Case-insensitive string does not Starts with |
