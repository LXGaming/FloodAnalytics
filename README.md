# FloodAnalytics

[![License](https://img.shields.io/github/license/LXGaming/FloodAnalytics?label=License&cacheSeconds=86400)](https://github.com/LXGaming/FloodAnalytics/blob/main/LICENSE)
[![Docker Hub](https://img.shields.io/docker/v/lxgaming/floodanalytics/latest?label=Docker%20Hub)](https://hub.docker.com/r/lxgaming/floodanalytics)

## Prerequisites
- [Flood](https://flood.js.org/)
- [Grafana](https://grafana.com/)
- [InfluxDB](https://www.influxdata.com/)

## Usage
### docker-compose
Download and use [config.json](https://raw.githubusercontent.com/LXGaming/FloodAnalytics/main/LXGaming.FloodAnalytics/config.json)
```yaml
services:
  floodanalytics:
    container_name: floodanalytics
    image: lxgaming/floodanalytics:latest
    restart: unless-stopped
    volumes:
      - /path/to/floodanalytics/logs:/app/logs
      - /path/to/floodanalytics/config.json:/app/config.json
```

## License
FloodAnalytics is licensed under the [Apache 2.0](https://github.com/LXGaming/FloodAnalytics/blob/main/LICENSE) license.