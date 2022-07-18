# followed: https://docs.docker.com/language/nodejs/build-images/
# and: https://create-react-app.dev/docs/deployment/
# and: https://stackoverflow.com/questions/63724148/creating-react-application-for-production-with-docker-build

FROM node:16.15.1-alpine3.16
ENV NODE_ENV=production

WORKDIR /app

ADD "package.json" .
ADD "package-lock.json" .
ADD "config-overrides.js" .

RUN npm install -g serve
RUN npm ci
COPY . .
RUN npm run build

EXPOSE 3000

CMD ["serve", "-s", "/app/build", "-l", "3000"]