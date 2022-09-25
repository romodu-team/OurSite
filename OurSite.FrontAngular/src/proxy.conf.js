const PROXY_CONFIG = [
  {
    context: [
      "/",
    ],
    target: "https://localhost:7181",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
