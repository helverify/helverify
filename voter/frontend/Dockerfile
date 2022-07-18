# followed: https://docs.docker.com/language/nodejs/build-images/
# and: https://mherman.org/blog/dockerizing-a-react-app/

FROM node:16.15.1-alpine3.16
ENV NODE_ENV=production

WORKDIR /app

COPY ["package.json", "package-lock.json", "./"]

RUN npm install -g serve
RUN npm install
RUN npm run build

COPY . .

EXPOSE 3000

CMD ["serve", "-s", "build", "-l", "3000"]