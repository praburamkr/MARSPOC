#Docker file
FROM node:alpine AS builder

WORKDIR /app

COPY . .

RUN npm install -g @angular/cli && npm install && \
    npm run prod

FROM nginx:alpine

COPY --from=builder /app/dist/* /usr/share/nginx/html/
