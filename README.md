# FloodAnalytics

[![License](https://lxgaming.github.io/badges/License-Apache%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![Docker Pulls](https://img.shields.io/docker/pulls/lxgaming/floodanalytics)](https://hub.docker.com/r/lxgaming/floodanalytics)

## Prerequisites
- InfluxDB
- Grafana

## Usage
### docker-compose
```yaml
version: "3"
services:
  floodanalytics:
    container_name: floodanalytics
    environment:
      - TZ=Pacific/Auckland
    image: lxgaming/floodanalytics:latest
    restart: unless-stopped
    volumes:
      - /path/to/floodanalytics/logs:/app/logs
      - /path/to/floodanalytics/config.json:/app/config.json
```

## License
FloodAnalytics is licensed under the [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0) license.