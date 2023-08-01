from grpc.tools import protoc
protoc.main(
    (
        '',
        '-I../Game.Api/protos/',
        '--python_out=./src/',
        '--grpc_python_out=./src/',
        'agent.proto',
    )
)
