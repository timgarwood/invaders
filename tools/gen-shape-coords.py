import sys

if(len(sys.argv) < 2):
    print("Usage:  gen-shape-coords <input file>")
    exit(1)

inputFile = open(sys.argv[1])

lines = inputFile.readlines()

inputFile.close()

center = lines[0].split(",")
coords = []

for i in range(0,len(lines)):
    lines[i] = lines[i].strip()
    if i == 0:
        center = lines[i].split(",")
    else:
        coords.append(lines[i].split(","))

offsets = []

print("\"vertices\": [")
for i in range(0, len(coords)):
    x = int(coords[i][0]) - int(center[0])
    y = int(coords[i][1]) - int(center[1])

    print("{\"X\":" + str(x) + ",\"Y\":" + str(y)+"},")

print("]")